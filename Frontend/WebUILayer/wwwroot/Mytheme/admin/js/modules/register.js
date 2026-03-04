/**
 * Register Page Script
 * Handles password visibility toggle, password strength checker
 */

document.addEventListener('DOMContentLoaded', function () {
    // Password visibility toggle for password field
    const togglePasswordBtn = document.getElementById('togglePassword');
    if (togglePasswordBtn) {
        togglePasswordBtn.addEventListener('click', function () {
            const passwordInput = document.getElementById('password'); // Dikkat: id css ile aynı olsun, C#'taki asp-for id'si
            if (!passwordInput) return;
            const icon = this.querySelector('i');
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.classList.replace('fa-eye', 'fa-eye-slash');
            } else {
                passwordInput.type = 'password';
                icon.classList.replace('fa-eye-slash', 'fa-eye');
            }
        });
    }

    // Password visibility toggle for confirm password field
    const toggleConfirmPasswordBtn = document.getElementById('toggleConfirmPassword');
    if (toggleConfirmPasswordBtn) {
        toggleConfirmPasswordBtn.addEventListener('click', function () {
            const passwordInput = document.getElementById('confirmPassword'); // Dikkat: id css ile aynı olsun, C#'taki asp-for id'si
            if (!passwordInput) return;
            const icon = this.querySelector('i');
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.classList.replace('fa-eye', 'fa-eye-slash');
            } else {
                passwordInput.type = 'password';
                icon.classList.replace('fa-eye-slash', 'fa-eye');
            }
        });
    }

    // Password strength checker
    const passwordInput = document.getElementById('password'); // id="password" olduğundan emin ol!
    const strengthFill = document.getElementById('strengthFill');
    const strengthText = document.getElementById('strengthText');

    if (passwordInput && strengthFill && strengthText) {
        passwordInput.addEventListener('input', function () {
            const password = this.value;
            let strength = 0;

            if (password.length >= 8) strength += 1; // 8 Karakter (1 Puan)
            if (/[a-z]/.test(password)) strength += 1; // Küçük harf (1 Puan)
            if (/[A-Z]/.test(password)) strength += 1; // Büyük harf (1 Puan)
            if (/\d/.test(password)) strength += 1; // Rakam (1 Puan)
            if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) strength += 1; // Orijinal (1 Puan)

            strengthFill.className = 'password-strength-fill';

            if (password.length === 0) {
                strengthText.textContent = 'En az 8 karakter';
            } else if (strength <= 2) {
                strengthFill.classList.add('weak');
                strengthText.textContent = 'Zayıf şifre';
            } else if (strength === 3 || strength === 4) {
                strengthFill.classList.add('medium');
                strengthText.textContent = 'Orta güçlükte';
            } else if (strength === 5) {
                strengthFill.classList.add('strong');
                strengthText.textContent = 'Güçlü şifre';
            }
        });
    }
});
