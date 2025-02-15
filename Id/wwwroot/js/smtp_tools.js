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
		Host: host,
		Port: parseInt(port, 10),  // Convert Port to an integer
		Username: username,
		Password: password,
		SenderEmail: senderEmail,
		SenderName: senderName,
		Secure: parseInt(sso, 10),  // Convert Secure to an integer
		AuthorizationRequired: authorizationRequired === "true"  // Convert string "true"/"false" to boolean
	});
	console.log(body);
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
			let res = {};
			res.successful = false;
			res.message = `HTTP error! status: ${response.status}`;
			return res;
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
	const body = JSON.stringify({ host: host, SmtpUsername: username, SmtpPassword: password });
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
			const data = {};
			data.successful = false;
			data.message = "Error in the HTTP connection to the server";
			return data;
		}
		const data = await response.json();
		return data;
	}
	catch (error) {
		const data = {};
		data.successful = false;
		data.message = "Unknown error:" + error;
		console.error("Error:", error);
		return data;
	}
}