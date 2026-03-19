import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getBiens } from '../services/bienService';
import { useAuth } from '../context/AuthContext';

export default function Biens() {
  const { utilisateur, logout } = useAuth();
  const navigate = useNavigate();
  const [biens, setBiens] = useState([]);
  const [loading, setLoading] = useState(true);
  const [erreur, setErreur] = useState('');
  const [filtres, setFiltres] = useState({ ville: '', type: '', prixMax: '' });

  const chargerBiens = async () => {
    setLoading(true);
    try {
      const params = {};
      if (filtres.ville) params.ville = filtres.ville;
      if (filtres.type) params.type = filtres.type;
      if (filtres.prixMax) params.prixMax = filtres.prixMax;
      const data = await getBiens(params);
      setBiens(data);
    } catch {
      setErreur('Erreur lors du chargement des biens.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    chargerBiens();
  }, []);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div style={styles.container}>
      {/* Navbar */}
      <nav style={styles.navbar}>
        <h1 style={styles.logo}>Ymmo</h1>
        <div style={styles.navRight}>
          <span style={styles.userInfo}>
            {utilisateur?.prenom} {utilisateur?.nom} — {utilisateur?.role}
          </span>
          <button onClick={handleLogout} style={styles.logoutBtn}>
            Déconnexion
          </button>
        </div>
      </nav>

      <div style={styles.content}>
        <h2>Biens immobiliers</h2>

        {/* Filtres */}
        <div style={styles.filtres}>
          <input
            placeholder="Ville"
            value={filtres.ville}
            onChange={(e) => setFiltres({ ...filtres, ville: e.target.value })}
            style={styles.input}
          />
          <select
            value={filtres.type}
            onChange={(e) => setFiltres({ ...filtres, type: e.target.value })}
            style={styles.input}
          >
            <option value="">Tous les types</option>
            <option value="Appartement">Appartement</option>
            <option value="Maison">Maison</option>
            <option value="Bureau">Bureau</option>
            <option value="Commerce">Commerce</option>
            <option value="Terrain">Terrain</option>
          </select>
          <input
            placeholder="Prix max"
            type="number"
            value={filtres.prixMax}
            onChange={(e) => setFiltres({ ...filtres, prixMax: e.target.value })}
            style={styles.input}
          />
          <button onClick={chargerBiens} style={styles.boutonFiltrer}>
            Filtrer
          </button>
        </div>

        {/* Liste */}
        {loading && <p>Chargement...</p>}
        {erreur && <p style={styles.erreur}>{erreur}</p>}

        <div style={styles.grille}>
          {biens.map((bien) => (
            <div key={bien.id} style={styles.carte}>
              <div style={styles.carteHeader}>
                <span style={styles.type}>{bien.type}</span>
                <span style={styles.statut}>{bien.statut}</span>
              </div>
              <h3 style={styles.titreBien}>{bien.titre}</h3>
              <p style={styles.ville}>📍 {bien.ville} ({bien.codePostal})</p>
              <p style={styles.description}>{bien.description}</p>
              <div style={styles.carteFooter}>
                <span style={styles.prix}>
                  {bien.prix.toLocaleString('fr-FR')} €
                </span>
                <span style={styles.surface}>{bien.surface} m²</span>
                <span style={styles.pieces}>{bien.nombrePieces} pièces</span>
              </div>
              <p style={styles.agence}>🏢 {bien.nomAgence}</p>
            </div>
          ))}
        </div>

        {biens.length === 0 && !loading && (
          <p style={styles.aucun}>Aucun bien trouvé.</p>
        )}
      </div>
    </div>
  );
}

const styles = {
  container: { minHeight: '100vh', backgroundColor: '#f5f5f5' },
  navbar: {
    backgroundColor: '#2c3e50',
    padding: '16px 32px',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  logo: { color: 'white', margin: 0 },
  navRight: { display: 'flex', alignItems: 'center', gap: '16px' },
  userInfo: { color: '#bdc3c7', fontSize: '14px' },
  logoutBtn: {
    padding: '8px 16px',
    backgroundColor: '#e74c3c',
    color: 'white',
    border: 'none',
    borderRadius: '6px',
    cursor: 'pointer',
  },
  content: { padding: '32px' },
  filtres: { display: 'flex', gap: '12px', marginBottom: '24px', flexWrap: 'wrap' },
  input: { padding: '10px', borderRadius: '6px', border: '1px solid #ddd', fontSize: '14px' },
  boutonFiltrer: {
    padding: '10px 20px',
    backgroundColor: '#2c3e50',
    color: 'white',
    border: 'none',
    borderRadius: '6px',
    cursor: 'pointer',
  },
  grille: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
    gap: '24px',
  },
  carte: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '20px',
    boxShadow: '0 2px 10px rgba(0,0,0,0.08)',
  },
  carteHeader: { display: 'flex', justifyContent: 'space-between', marginBottom: '8px' },
  type: {
    backgroundColor: '#3498db',
    color: 'white',
    padding: '4px 10px',
    borderRadius: '20px',
    fontSize: '12px',
  },
  statut: {
    backgroundColor: '#2ecc71',
    color: 'white',
    padding: '4px 10px',
    borderRadius: '20px',
    fontSize: '12px',
  },
  titreBien: { margin: '8px 0', color: '#2c3e50' },
  ville: { color: '#7f8c8d', fontSize: '14px', margin: '4px 0' },
  description: { color: '#555', fontSize: '14px', margin: '8px 0' },
  carteFooter: { display: 'flex', gap: '12px', margin: '12px 0' },
  prix: { fontWeight: 'bold', color: '#2c3e50', fontSize: '18px' },
  surface: { color: '#7f8c8d', fontSize: '14px', alignSelf: 'center' },
  pieces: { color: '#7f8c8d', fontSize: '14px', alignSelf: 'center' },
  agence: { color: '#95a5a6', fontSize: '12px', margin: '4px 0' },
  erreur: { color: 'red' },
  aucun: { textAlign: 'center', color: '#7f8c8d', marginTop: '40px' },
};