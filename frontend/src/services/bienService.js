import api from './api';

export const getBiens = async (filtres = {}) => {
  const response = await api.get('/Biens', { params: filtres });
  return response.data;
};

export const getBienById = async (id) => {
  const response = await api.get(`/Biens/${id}`);
  return response.data;
};

export const creerBien = async (data) => {
  const response = await api.post('/Biens', data);
  return response.data;
};

export const modifierBien = async (id, data) => {
  const response = await api.put(`/Biens/${id}`, data);
  return response.data;
};

export const supprimerBien = async (id) => {
  await api.delete(`/Biens/${id}`);
};