﻿const admin_id = document.getElementById("nav_user");
const application_id = document.getElementById("nav_application");
const brand_id = document.getElementById("nav_brand");
const roles_id = document.getElementById("nav_roles");
const smtp_id = document.getElementById("nav_smtp");
const login_id = document.getElementById("nav_login");
const password_id = document.getElementById("nav_password");
const cookie_id = document.getElementById("nav_cookie");
const jwt_id = document.getElementById("nav_jwt");

window.onload = async () => {
    console.log("Loading: " + active_page);
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
    };
    loadHelp(help);
};