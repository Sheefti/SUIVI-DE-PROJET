import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import Login from './pages/Login';
import Biens from './pages/Biens';

function RouteProtegee({ children }) {
  const { utilisateur, loading } = useAuth();

  if (loading) return <p>Chargement...</p>;
  if (!utilisateur) return <Navigate to="/login" />;

  return children;
}

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route
        path="/biens"
        element={
          <RouteProtegee>
            <Biens />
          </RouteProtegee>
        }
      />
      <Route path="*" element={<Navigate to="/login" />} />
    </Routes>
  );
}