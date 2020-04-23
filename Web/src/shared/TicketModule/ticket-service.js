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

const revealTicket = async function (ticketId) {
    try {
        let resourcePath = `${API_URL}tickets/${ticketId}/reveal`;
        var response = await apiClient.post(resourcePath);
        return response;
    } catch (error) {
        return null;
    }
}

const getTicketById = async function (ticketId) {
    try {
        let resourcePath = `${API_URL}tickets/${ticketId}`;
        var response = await apiClient.get(resourcePath);
        return response;
    } catch (error) {
        return null;
    }
}

export const ticketService = {
    pushTicket,
    revealTicket,
    getTicketById
}