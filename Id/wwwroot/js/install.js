/**
 * References to navigation elements by their corresponding IDs.
 * These elements are used to highlight the active page in the navigation menu.
 */
const admin_id = document.getElementById("nav_user");
const application_id = document.getElementById("nav_application");
const brand_id = document.getElementById("nav_brand");
const roles_id = document.getElementById("nav_roles");
const smtp_id = document.getElementById("nav_smtp");
const login_id = document.getElementById("nav_login");
const password_id = document.getElementById("nav_password");
const cookie_id = document.getElementById("nav_cookie");
const jwt_id = document.getElementById("nav_jwt");
const cors_id = document.getElementById("nav_cors");
const nav_result = document.getElementById("nav_result");

/**
 * Executes when the page has fully loaded.
 * Highlights the active navigation link and attempts to load help and startup functions.
 */
window.onload = async () => {
    console.log("Loading: " + active_page);

    /**
     * Highlights the current active page in the navigation menu by underlining it.
     * @param {string} active_page - The current page identifier.
     */
    switch (active_page) {
        case "admin":
            admin_id.style.textDecoration = "underline";
            break;
        case "application":
            application_id.style.textDecoration = "underline";
            break;
        case "brand":
            brand_id.style.textDecoration = "underline";
            break;
        case "roles":
            roles_id.style.textDecoration = "underline";
            break;
        case "smtp":
            smtp_id.style.textDecoration = "underline";
            break;
        case "login":
            login_id.style.textDecoration = "underline";
            break;
        case "password":
            password_id.style.textDecoration = "underline";
            break;
        case "cookie":
            cookie_id.style.textDecoration = "underline";
            break;
        case "jwt":
            jwt_id.style.textDecoration = "underline";
            break;
        case "cors":
            cors_id.style.textDecoration = "underline";
            break;
        case "result":
            nav_result.style.textDecoration = "underline";
            break;
    }

    /**
     * Attempts to load contextual help content for the current page.
     * If the `loadHelp` function is unavailable, an error is logged.
     */
    try {
        loadHelp(help);
    } catch {
        console.log("No help function");
    }

    /**
     * Attempts to execute the `startup` function if it exists.
     * This function may contain initialization logic for the page.
     */
    try {
        await startup();
    } catch {
        console.log("No startup function");
    }
};
