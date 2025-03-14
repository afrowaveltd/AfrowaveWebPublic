/**
 * Tests SMTP settings by sending a request to the SMTP test API.
 * If no target for testing is provided, it defaults to the sender email.
 * @param {string} host - SMTP server host.
 * @param {number|string} port - SMTP server port (converted to integer).
 * @param {string} username - SMTP account username.
 * @param {string} password - SMTP account password.
 * @param {string} senderEmail - Email address used as sender.
 * @param {string} senderName - Display name for sender.
 * @param {number|string} sso - Security option (converted to integer, e.g., SSL, TLS).
 * @param {string|boolean} authorizationRequired - Indicates whether authorization is required ("true"/"false").
 * @param {string} [targetForTesting] - Optional email address to send the test email to.
 * @returns {Promise<object>} - An object indicating success status and message.
 */
const testSmtp = async (host,
	port,
	username,
	password,
	senderEmail,
	senderName,
	sso,
	authorizationRequired,
	targetForTesting) => {
	// now we create json body of the post request
	if (!targetForTesting || targetForTesting === "") {
		targetForTesting = senderEmail;
	}
	const body = JSON.stringify({
		Host: host,
		Port: parseInt(port, 10),  // Convert Port to an integer
		Username: username,
		Password: password,
		SenderEmail: senderEmail,
		SenderName: senderName,
		Secure: parseInt(sso, 10),  // Convert Secure to an integer
		AuthorizationRequired: authorizationRequired === "true",
		TargetForTesting: targetForTesting
		// Convert string "true"/"false" to boolean
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
};

/**
 * Attempts to automatically detect SMTP settings using the provided host, username, and password.
 * Sends the credentials to the SMTP autodetect API and returns the detected settings.
 * @param {string} host - SMTP server host.
 * @param {string} [username=""] - Optional SMTP account username.
 * @param {string} [password=""] - Optional SMTP account password.
 * @returns {Promise<object>} - An object indicating success status and message, possibly with detected settings.
 */
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
};