import axios from 'axios';
import vm from '@/main';

const apiClient = {
    async get(resource, jwt = null) {
        try {
            const requestResult = await axios.get(resource, { jwt });
            var responseData = this.processResponse(requestResult);
            return responseData;
        } catch (error) {
            vm.$snotify.error(error.message);
            return null;
        }
    },
    async post(resource, data, jwt) {
        try {
            const requestResult = await axios.post(resource, data, { jwt });
            var responseData = this.processResponse(requestResult);
            return responseData;
        } catch (error) {
            vm.$snotify.error(error.message);
            return null;
        }
    },
    processResponse(response) {
        if (response.status !== 200) {
            throw Error(response.message);
        }
        return response.data;
    }
}

export default apiClient;
