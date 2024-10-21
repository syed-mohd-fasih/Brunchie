const toggleDarkMode = document.getElementById('toggleDarkMode');
const themeIcon = document.getElementById('themeIcon');

if (localStorage.getItem('darkMode') === 'enabled') {
    document.body.classList.add('dark-mode');
    themeIcon.classList.replace('fa-moon', 'fa-sun');
}

toggleDarkMode.addEventListener('click', () => {
    document.body.classList.toggle('dark-mode');
    if (document.body.classList.contains('dark-mode')) {
        localStorage.setItem('darkMode', 'enabled');
        themeIcon.classList.replace('fa-moon', 'fa-sun');
    } else {
        localStorage.setItem('darkMode', 'disabled');
        themeIcon.classList.replace('fa-sun', 'fa-moon');
    }
});