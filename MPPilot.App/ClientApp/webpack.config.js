const path = require('path');
const { VueLoaderPlugin } = require("vue-loader");

module.exports = {
    entry: {
        site: './src/js/index.js'
    },
    output: {
        filename: 'mppilot.entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist')
    },
    devtool: 'source-map',
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
                type: 'asset'
            },
            {
                test: /\.vue$/,
                loader: "vue-loader",
            }
        ]
    },
    plugins: [
        new VueLoaderPlugin()
    ],
    resolve: {
        extensions: ['.tsx', '.ts', '.js', '.vue'],
        alias: {
            vue: 'vue/dist/vue.js'
        }
    }
};