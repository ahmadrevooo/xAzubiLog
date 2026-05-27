window.myTheme = {
    setTheme: function (isDark) {

        document.body.classList.remove("theme-dark", "theme-light");

        if (isDark) {
            document.body.classList.add("theme-dark");
        } else {
            document.body.classList.add("theme-light");
        }

        localStorage.setItem("theme", isDark ? "dark" : "light");
    },
    getStoredTheme: function () {
        return localStorage.getItem("theme");
    }
};

// expose friendly method used by server-side code
window.setTheme = function(isDark) { window.myTheme.setTheme(isDark); };
