import axios from 'axios';
import vm from '@/main';

axios.interceptors.request.use(config => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (user != null) {
        config.headers.post['Authorization'] = `Bearer ${user.authenticationToken}`;
    }
    return config;
});

const apiClient = {
    async get(resource) {
        var response = null;
        await axios.get(resource)
            .then(r => {
                response = r.data;
            })
            .catch(error => {
                informAboutAnError(error.response);
                throw error;
            });
        return response;
    },
    async post(resource, data) {
        var response = null;
        await axios.post(resource, data)
            .then(r => {
                response = r.data;
            })
            .catch(error => {
                informAboutAnError(error.response);
                throw error;
            });
        return response;
    }
}

function informAboutAnError(error) {
    if (error.status !== 500) {
        error.data.forEach(e => {
            vm.$snotify.error(e.message);
        });
    } else {
        vm.$snotify.error(error.data.message);
    }
}

export default apiClient;
