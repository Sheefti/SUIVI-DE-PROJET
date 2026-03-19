import { createContext, useContext, useState, useEffect } from 'react';
import { login as loginService } from '../services/authService';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [utilisateur, setUtilisateur] = useState(null);
  const [loading, setLoading] = useState(true);

  // Au démarrage, on récupère l'utilisateur stocké
  useEffect(() => {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('utilisateur');
    if (token && user) {
      setUtilisateur(JSON.parse(user));
    }
    setLoading(false);
  }, []);

  const login = async (email, motDePasse) => {
    const data = await loginService(email, motDePasse);
    localStorage.setItem('token', data.token);
    localStorage.setItem('utilisateur', JSON.stringify({
      nom: data.nom,
      prenom: data.prenom,
      email: data.email,
      role: data.role,
    }));
    setUtilisateur({
      nom: data.nom,
      prenom: data.prenom,
      email: data.email,
      role: data.role,
    });
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('utilisateur');
    setUtilisateur(null);
  };

  return (
    <AuthContext.Provider value={{ utilisateur, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);