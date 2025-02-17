﻿// Set cookie with a default expiration of 100 years
const setCookie = (name, value, expirationDays = 36500) => {
	const d = new Date();
	d.setTime(d.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
	const expires = `expires=${d.toUTCString()}`;
	document.cookie = `${name}=${encodeURIComponent(value)};${expires};path=/`;
};

// Get cookie by name
const getCookie = (name) => {
	const decodedCookie = decodeURIComponent(document.cookie);
	const cookies = decodedCookie.split(';');
	for (const cookie of cookies) {
		const [key, value] = cookie.trim().split('=');
		if (key === name) {
			return value || '';
		}
	}
	return '';
};

// Change language and reload the page if necessary
const changeLanguage = (language) => {
	language = language.toLowerCase();
	setCookie('language', language); // Use consistent cookie naming
	location.reload(); // Reload only if the cookie is different
};
// Apply the theme without reloading
const applyTheme = (theme) => {
	theme = theme.toLowerCase();
	document.documentElement.setAttribute('data-theme', theme);
	location.reload();
};

// Change theme and apply immediately
const setTheme = (theme) => {
	theme = theme.toLowerCase();
	setCookie('theme', theme); // Use consistent cookie naming
	applyTheme(theme);
};

// Debounce function to limit the number of API requests
const debounce = (func, onError) => {
	console.log("Debouncing");
	const delay = 800; // 0.8 second
	let timeoutId; // Stores the timer ID

	return function (...args) {
		// Clear the previous timer if it exists
		clearTimeout(timeoutId);

		// Set a new timer
		timeoutId = setTimeout(async () => {
			try {
				await func.apply(this, args);
				console.log("triggering function");
			} catch (error) {
				if (onError && typeof onError === 'function') {
					onError(error);
				} else {
					console.error('Error in debounced function:', error);
				}
			}
		}, delay);
	};
};