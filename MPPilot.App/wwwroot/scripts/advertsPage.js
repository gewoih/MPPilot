import { createApp } from 'vue';
import axios from 'axios';

const app = createApp({
	data() {
		return {
			adverts: [],
			isLoading: true,
			oldAdvert: null,
			selectedAdvert: {
				name: '',
				keyword: '',
				isEnabled: false,
				autobidder: {
					mode: '0',
					dailyBudget: 0,
					isEnabled: false
				}
			},
			advertId: null
		};
	},

	mounted() {
		this.fetchAdverts();
	},

	methods: {
		fetchAdverts() {
			fetch('/adverts/getAdverts')
				.then(response => response.json())
				.then(data => {
					this.adverts = data;
					this.isLoading = false;
				})
				.catch(error => {
					console.error('Error fetching data:', error);
				})
		},

		openModal(advert) {
			if (advert.autobidder === null) {
				advert.autobidder = {
					mode: '0',
					dailyBudget: 0,
					isEnabled: false
				};
			}

			this.selectedAdvert = advert;
			this.oldAdvert = { ...advert };
		},

		saveSettings() {
			const headers = {
				'Content-Type': 'application/json'
			};

			const status = this.selectedAdvert.isEnabled ? 9 : 11;
			this.selectedAdvert.status = status;

			const advertsData = {
				oldAdvert: this.oldAdvert,
				newAdvert: this.selectedAdvert
			};

			axios.post('/adverts/edit', JSON.stringify(advertsData), { headers })
				.then(() => {
					return axios.post('/autobidder/edit', JSON.stringify(this.selectedAdvert.autobidder), { headers })
						.then(() => {
							alert('Настройки успешно сохранены!');
							return this.fetchAdverts();
						});
				});
		}
	}
});

app.mount("#advertsPage");
