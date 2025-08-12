const tokenBtn   = document.getElementById('getToken');
const tokenText  = document.getElementById('tokenText');
const tokenInput = document.getElementById('tokenInput');

const weatherBtn = document.getElementById('getWeather');
const weatherP   = document.getElementById('weatherText');

// --- POST /auth/login ---
tokenBtn.addEventListener('click', async () => {
tokenText.textContent = 'Chargement...';
    try {
        const res = await fetch('https://localhost:7175/auth/login', {
          method: 'POST',
          headers: {
            'Accept': 'application/json'
            // Ajoute 'Content-Type': 'application/json' et un body si ton endpoint attend des identifiants
            // 'Content-Type': 'application/json'
          },
          // body: JSON.stringify({ username: 'sandrine', password: '***' })
        });

        if (!res.ok) {
          const body = await res.text().catch(() => '');
          tokenText.textContent =
            `Erreur HTTP: ${res.status} ${res.statusText}\n` +
            (body ? `\nRéponse:\n${body}` : '');
          return;
        }

        let raw = await res.text();
        let data;
        try { data = JSON.parse(raw); } catch { data = raw; }

        const findToken = (obj) => {
          if (!obj || typeof obj !== 'object') return null;
          return obj.token ?? obj.access_token ?? obj.jwt ?? obj.result?.token ?? null;
        };

        if (typeof data === 'object') {
          const tok = findToken(data);
          if (tok) {
            tokenText.textContent = tok;
          }
        } 
      } catch (err) {
        tokenText.textContent = `Erreur de connexion: ${err}`;
      }
    });

// --- GET /weather avec Authorization: Bearer <token> ---
weatherBtn.addEventListener('click', async () => {
    const token = tokenInput.value.trim();
    weatherP.textContent = 'Chargement...';

    try {
        const res = await fetch('https://localhost:7175/weather', {
        method: 'GET',
          headers: {
            'Accept': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {})
          }
        });

        if (!res.ok) {
          const body = await res.text().catch(() => '');
          weatherP.textContent =
            `Erreur HTTP: ${res.status} ${res.statusText}\n` +
            (body ? `\nRéponse:\n${body}` : '');
          return;
        }

        const text = await res.text();
        try {
          const json = JSON.parse(text);
          weatherP.textContent = JSON.stringify(json, null, 2);
        } catch {
          weatherP.textContent = text || 'Réponse vide';
        }
      } catch (err) {
        weatherP.textContent = `Erreur de connexion: ${err}`;
      }
});