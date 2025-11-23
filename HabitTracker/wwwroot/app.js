const loginForm = document.getElementById('login-form');
const errorMsg = document.getElementById('error-msg');
const languageSelect = document.getElementById('language');
const title = document.getElementById('title');
const loginBtn = document.getElementById('login-btn');
const registerLink = document.getElementById('register-link');

const texts = {
    pl: {
        login: "Logowanie",
        email: "Email",
        password: "Hasło",
        loginBtn: "Zaloguj",
        registerText: "Nie masz konta? Zarejestruj się",
        error: "Nieprawidłowy email lub hasło!"
    },
    en: {
        login: "Login",
        email: "Email",
        password: "Password",
        loginBtn: "Sign In",
        registerText: "Don't have an account? Register",
        error: "Invalid email or password!"
    }
};

// Zmiana języka
languageSelect.addEventListener('change', () => {
    const lang = languageSelect.value;
    title.textContent = texts[lang].login;
    document.getElementById('email').placeholder = texts[lang].email;
    document.getElementById('password').placeholder = texts[lang].password;
    loginBtn.textContent = texts[lang].loginBtn;
    registerLink.parentElement.innerHTML = texts[lang].registerText.replace("Register", `<a href="#" id="register-link">Register</a>`);
});

// Logowanie
loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('http://localhost:5130/api/account/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            throw new Error("Invalid credentials");
        }

        const data = await response.json();
        localStorage.setItem('token', data.token);

        // Przekierowanie do habits po zalogowaniu
        window.location.href = 'habits.html';

    } catch (err) {
        const lang = languageSelect.value;
        errorMsg.textContent = texts[lang].error;
    }
});
