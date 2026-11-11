(function () {
    function ensureAuth() {
        const token = localStorage.getItem('fitlife_token');
        if (!token) {
            window.location.href = '../../index.html';
            return null;
        }
        return token;
    }

    async function apiFetch(url, options = {}) {
        const token = ensureAuth();
        if (!token) return null;
        const { headers = {}, ...rest } = options;

        try {
            const response = await fetch(url, {
                ...rest,
                headers: {
                    'Content-Type': 'application/json',
                    Accept: 'application/json',
                    ...headers,
                    Authorization: `Bearer ${token}`
                }
            });

            if (response.status === 401 || response.status === 403) {
                localStorage.removeItem('fitlife_token');
                localStorage.removeItem('fitlife_user');
                window.location.href = '../../index.html';
                return null;
            }

            if (response.status === 204) return null;
            const text = await response.text();
            if (!text) return null;

            try {
                return JSON.parse(text);
            } catch (error) {
                return text;
            }
        } catch (error) {
            console.error('API Error:', error);
            return null;
        }
    }
})();
