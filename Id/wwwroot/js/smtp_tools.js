const testSmtp = async (host,
	port,
	username,
	password,
	senderEmail,
	senderName,
	sso,
	authorizationRequired) => {
}

const detectSmtp = async (host, username = "", password = "") => {
	// now we create json body of the post request
	const body = JSON.stringify({ host: host, username: username, password: password });
	const url = "/api/smtp/autodetect";
}