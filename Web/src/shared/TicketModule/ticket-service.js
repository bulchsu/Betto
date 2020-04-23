import { API_URL } from '../config';
import apiClient from '../api-client';

const pushTicket = async function (ticket) {
    try {
        let resourcePath = `${API_URL}tickets/`;
        var response = await apiClient.post(resourcePath, { userId: ticket.userId, stake: ticket.stake, events: ticket.events });
        return response;
    } catch (error) {
        return null;
    }
}

export const ticketService = {
    pushTicket
}