window.myTheme = {
    setTheme: function (isDark) {

        console.log("SWITCH:", isDark);

        document.body.classList.remove("theme-dark", "theme-light");

        if (isDark) {
            document.body.classList.add("theme-dark");
        } else {
            document.body.classList.add("theme-light");
        }

        localStorage.setItem("theme", isDark ? "dark" : "light");
    }
};