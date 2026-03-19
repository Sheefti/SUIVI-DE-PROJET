import api from './api';

export const login = async (email, motDePasse) => {
  const response = await api.post('/Auth/login', {
    email,
    motDePasse,
  });
  return response.data;
};

export const register = async (data) => {
  const response = await api.post('/Auth/register', data);
  return response.data;
};