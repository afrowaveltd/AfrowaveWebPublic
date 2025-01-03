﻿const testSmtp = async (host,
	port,
	username,
	password,
	senderEmail,
	senderName,
	sso,
	authorizationRequired) => {
	// now we create json body of the post request
	const body = JSON.stringify({
		host: host,
		port: port,
		username: username,
		password: password,
		senderEmail: senderEmail,
		senderName: senderName,
		sso: sso,
		authorizationRequired: authorizationRequired
	});
	const url = "/api/smtp/test";
	try {
		const response = await fetch(url, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: body,
		});
		if (!response.ok) {
			throw new Error(`HTTP error! status: ${response.status}`);
		}
		const data = await response.json();
		return data;
	}
	catch (error) {
		console.error("Error:", error);
	}
}

const detectSmtp = async (host, username = "", password = "") => {
	// now we create json body of the post request
	const body = JSON.stringify({ host: host, username: username, password: password });
	const url = "/api/smtp/autodetect";
	try {
		const response = await fetch(url, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: body,
		});
		if (!response.ok) {
			throw new Error(`HTTP error! status: ${response.status}`);
		}
		const data = await response.json();
		return data;
	}
	catch (error) {
		console.error("Error:", error);
	}
}