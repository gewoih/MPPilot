import { inject, provide, watch, defineComponent, onMounted, onUpdated, onBeforeUnmount, computed, h, reactive, readonly, createApp, getCurrentInstance } from "vue";
const wormholeSymbol = Symbol("wormhole");
function useWormhole() {
  const wh = inject(wormholeSymbol);
  if (!wh) {
    throw new Error(`
    [portal-vue]: Necessary Injection not found. Make sur you installed the plugin properly.`);
  }
  return wh;
}
function provideWormhole(wormhole2) {
  provide(wormholeSymbol, wormhole2);
}
const inBrowser = typeof window !== "undefined";
function warn(msg) {
  console.log("[portal-vue]: " + msg);
}
function assertStaticProps(component, props, propNames) {
  propNames.forEach(
    (name) => {
      watch(
        () => props[name],
        () => {
          warn(
            `Prop '${name}' of component ${component} is static, but was dynamically changed by the parent.
          This change will not have any effect.`
          );
        }
      );
    },
    { flush: "post" }
  );
}
function stableSort(array, compareFn) {
  return array.map((v, idx) => {
    return [idx, v];
  }).sort(function(a, b) {
    return compareFn(a[1], b[1]) || a[0] - b[0];
  }).map((c) => c[1]);
}
function usePortal(props, slots) {
  const wormhole2 = useWormhole();
  function sendUpdate() {
    if (!inBrowser)
      return;
    const { to, name: from, order } = props;
    if (slots.default) {
      wormhole2.open({
        to,
        from,
        order,
        content: slots.default
      });
    } else {
      clear();
    }
  }
  function clear(target) {
    wormhole2.close({
      to: target ?? props.to,
      from: props.name
    });
  }
  onMounted(() => {
    if (!props.disabled) {
      sendUpdate();
    }
  });
  onUpdated(() => {
    if (props.disabled) {
      clear();
    } else {
      sendUpdate();
    }
  });
  onBeforeUnmount(() => {
    clear();
  });
  watch(
    () => props.to,
    (newTo, oldTo) => {
      if (props.disabled)
        return;
      if (oldTo && oldTo !== newTo) {
        clear(oldTo);
      }
      sendUpdate();
    }
  );
}
const Portal = defineComponent({
  compatConfig: { MODE: 3 },
  name: "portal",
  props: {
    disabled: { type: Boolean },
    name: { type: [String, Symbol], default: () => Symbol() },
    order: { type: Number },
    slotProps: { type: Object, default: () => ({}) },
    to: {
      type: String,
      default: () => String(Math.round(Math.random() * 1e7))
    }
  },
  setup(props, { slots }) {
    assertStaticProps("Portal", props, ["order", "name"]);
    usePortal(props, slots);
    return () => {
      if (props.disabled && slots.default) {
        return slots.default(props.slotProps);
      } else {
        return null;
      }
    };
  }
});
const PortalTargetContent = (_, { slots }) => {
  var _a;
  return (_a = slots.default) == null ? void 0 : _a.call(slots);
};
const PortalTarget = defineComponent({
  compatConfig: { MODE: 3 },
  name: "portalTarget",
  props: {
    multiple: { type: Boolean, default: false },
    name: { type: String, required: true },
    slotProps: { type: Object, default: () => ({}) }
  },
  emits: ["change"],
  setup(props, { emit, slots }) {
    const wormhole2 = useWormhole();
    const slotVnodes = computed(
      () => {
        const transports = wormhole2.getContentForTarget(
          props.name,
          props.multiple
        );
        const wrapperSlot = slots.wrapper;
        const rawNodes = transports.map((t) => t.content(props.slotProps));
        const vnodes = wrapperSlot ? rawNodes.flatMap(
          (nodes) => nodes.length ? wrapperSlot(nodes) : []
        ) : rawNodes.flat(1);
        return {
          vnodes,
          vnodesFn: () => vnodes
        };
      }
    );
    watch(
      slotVnodes,
      ({ vnodes }) => {
        const hasContent = vnodes.length > 0;
        const content = wormhole2.transports.get(props.name);
        const sources = content ? [...content.keys()] : [];
        emit("change", { hasContent, sources });
      },
      { flush: "post" }
    );
    return () => {
      var _a;
      const hasContent = !!slotVnodes.value.vnodes.length;
      if (hasContent) {
        return [
          h("div", {
            style: "display: none",
            key: "__portal-vue-hacky-scoped-slot-repair__"
          }),
          h(PortalTargetContent, slotVnodes.value.vnodesFn)
        ];
      } else {
        return (_a = slots.default) == null ? void 0 : _a.call(slots);
      }
    };
  }
});
function createWormhole(asReadonly = true) {
  const transports = reactive(/* @__PURE__ */ new Map());
  function open(transport) {
    if (!inBrowser)
      return;
    const { to, from, content, order = Infinity } = transport;
    if (!to || !from || !content)
      return;
    if (!transports.has(to)) {
      transports.set(to, /* @__PURE__ */ new Map());
    }
    const transportsForTarget = transports.get(to);
    const newTransport = {
      to,
      from,
      content,
      order
    };
    transportsForTarget.set(from, newTransport);
  }
  function close(transport) {
    const { to, from } = transport;
    if (!to || !from)
      return;
    const transportsForTarget = transports.get(to);
    if (!transportsForTarget) {
      return;
    }
    transportsForTarget.delete(from);
    if (!transportsForTarget.size) {
      transports.delete(to);
    }
  }
  function getContentForTarget(target, returnAll) {
    const transportsForTarget = transports.get(target);
    if (!transportsForTarget)
      return [];
    const content = Array.from((transportsForTarget == null ? void 0 : transportsForTarget.values()) || []);
    if (!returnAll) {
      return [content.pop()];
    }
    return stableSort(
      content,
      (a, b) => a.order - b.order
    );
  }
  const wh = {
    open,
    close,
    transports,
    getContentForTarget
  };
  return asReadonly ? readonly(wh) : wh;
}
const wormhole = createWormhole();
function mountPortalTarget(targetProps, el) {
  const app = createApp({
    render: () => h(PortalTarget, targetProps)
  });
  if (!targetProps.multiple) {
    const provides = getCurrentInstance().provides ?? {};
    app._context.provides = Object.create(provides);
  }
  onMounted(() => {
    app.mount(el);
  });
  onBeforeUnmount(() => {
    app.unmount();
  });
}
function install(app, options = {}) {
  options.portalName !== false && app.component(options.portalName || "Portal", Portal);
  options.portalTargetName !== false && app.component(options.portalTargetName || "PortalTarget", PortalTarget);
  const wormhole$1 = options.wormhole ?? wormhole;
  app.provide(wormholeSymbol, wormhole$1);
}
const Wormhole = wormhole;
const version = "3.0.0";
export {
  Portal,
  PortalTarget,
  Wormhole,
  createWormhole,
  install as default,
  install,
  mountPortalTarget,
  provideWormhole,
  useWormhole,
  version
};
//# sourceMappingURL=portal-vue.es.dev.mjs.map
