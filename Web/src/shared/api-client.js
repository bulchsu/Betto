import axios from 'axios';
import vm from '@/main';

const apiClient = {
    async get(resource, jwt = null) {
        try {
            const response = await axios.get(resource, { jwt });
            return response.data;
        } catch (error) {
            if (error.response.data && error.response.data.message) {
                vm.$snotify.error(error.response.data.message);
            }
            throw error;
        }
    },
    async post(resource, data, jwt) {
        try {
            const response = await axios.post(resource, data, { jwt });
            return response.data;
        } catch (error) {
            if (error.response.data && error.response.data.message) {
                vm.$snotify.error(error.response.data.message);
            }
            throw error;
        }
    }
}

export default apiClient;
