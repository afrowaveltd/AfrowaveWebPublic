let smtpHostOk = false;
let smtpPortOk = true;
let smtpUserOk = false;
let smtpPasswordOk = false;
let smtpSenderEmailOk = false;
let smtpSenderNameOk = true;
let smtpSecureConnectionOk = true;
let smtpUseAuthenticationOk = true;

let canAutodetect = false;
let canTest = false;
let canSave = false;

const smtpHostErr = document.getElementById('smtp_host_err');
const smtpPortErr = document.getElementById('smtp_port_err');
const smtpUserErr = document.getElementById('smtp_user_err');
const smtpPasswordErr = document.getElementById('smtp_pass_err');
const smtpSenderEmailErr = document.getElementById('smtp_email_err');
const smtpSenderNameErr = document.getElementById('smtp_name_err');
const smtpSecureConnectionErr = document.getElementById('smtp_sso_err');

const checkSmtpForm = () => {
	if (smtpHostOk && smtpPortOk && smtpUserOk && smtpPasswordOk && smtpSenderEmailOk && smtpSenderNameOk && smtpSecureConnectionOk && smtpUseAuthenticationOk) {
		canTest = true;
	} else {
		canTest = false;
	}
	if (smtpHostOk && document.getElementById('auth_required').value == "true") {
		canAutodetect = true;
	}
	else {
		canAutodetect = false;
	}

	if (canTest) {
		document.getElementById('testSmtp').removeAttribute('disabled');
	}
	else {
		document.getElementById('testSmtp').setAttribute('disabled', 'disabled');
	}

	if (canAutodetect) {
		document.getElementById('detectSmtp').removeAttribute('disabled');
	}
	else {
		document.getElementById('detectSmtp').setAttribute('disabled', 'disabled');
	}

	// check status of variables *Ok to colorize the input fields

	const smtpHost = document.getElementById('smtp_host');
	const smtpPort = document.getElementById('smtp_port');
	const smtpUser = document.getElementById('smtp_user');
	const smtpPassword = document.getElementById('smtp_pass');
	const smtpSendEmail = document.getElementById('smtp_email');
	const smtpSendName = document.getElementById('smtp_name');
	const smtpSecureConnection = document.getElementById('smtp_sso');
	const smtpUseAuthentication = document.getElementById('auth_required');

	if (smtpHostOk) {
		smtpHost.style.borderColor = 'green';
	} else {
		smtpHost.style.borderColor = 'red';
	}

	if (smtpPortOk) {
		smtpPort.style.borderColor = 'green';
	} else {
		smtpPort.style.borderColor = 'red';
	}

	if (smtpUserOk) {
		smtpUser.style.borderColor = 'green';
	} else {
		smtpUser.style.borderColor = 'red';
	}

	if (smtpPasswordOk) {
		smtpPassword.style.borderColor = 'green';
	} else {
		smtpPassword.style.borderColor = 'red';
	}

	if (smtpSenderEmailOk) {
		smtpSendEmail.style.borderColor = 'green';
	} else {
		smtpSendEmail.style.borderColor = 'red';
	}

	if (smtpSenderNameOk) {
		smtpSendName.style.borderColor = 'green';
	} else {
		smtpSendName.style.borderColor = 'red';
	}

	if (smtpSecureConnectionOk) {
		smtpSecureConnection.style.borderColor = 'green';
	} else {
		smtpSecureConnection.style.borderColor = 'red';
	}

	if (smtpUseAuthenticationOk) {
		smtpUseAuthentication.style.borderColor = 'green';
	} else {
		smtpUseAuthentication.style.borderColor = 'red';
	}

	if (canSave == true) {
		document.getElementById('submit').removeAttribute('disabled');
	}
	else {
		document.getElementById('submit').setAttribute('disabled', 'disabled');
	}
}

const checkSmtpHost = async (element) => {
	if (element.value.trim() === '') {
		smtpHostOk = false;
		smtpHostErr.innerHTML = await localize('SMTP Host is required');
	} else {
		smtpHostOk = true;
		smtpHostErr.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpPort = async (element) => {
	if (element.value.trim() === '' || element.value.trim() === "0") {
		smtpPortOk = false;
		smtpPortErr.innerHTML = await localize('SMTP port is required');
	} else {
		smtpPortOk = true;
		smtpPortErr.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpUser = async (element) => {
	const smtpUser = element;
	if (smtpUser.value.trim() === '' && document.getElementById('auth_required').value === "true") {
		smtpUserOk = false;
		smtpUserErr.innerHTML = await localize('SMTP user is required');
	} else {
		smtpUserOk = true;
		smtpUserErr.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpPassword = async (element) => {
	const smtpPassword = element;
	if (smtpPassword.value.trim() === '' && document.getElementById('auth_required').value === "true") {
		smtpPasswordOk = false;
		smtpPasswordErr.innerHTML = await localize('SMTP Password is required');
	} else {
		smtpPasswordOk = true;
		smtpPasswordErr.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpSenderEmail = async (element) => {
	if (element.value.trim() === '') {
		smtpSenderEmailOk = false;
		smtpSenderEmailErr.innerHTML = 'Sender Email is required';
	} else {
		smtpSenderEmailOk = true;
		smtpSenderEmailErr.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpSenderName = async (element) => {
	if (element.value.trim() === '') {
		let emailValue = document.getElementById('smtp_email').value;
		if (emailValue.trim() === '') {
			smtpSenderNameOk = false;
			smtpSenderNameErr.innerHTML = await localize('Sender name is required');
		} else {
			this.value = emailValue;
			smtpSenderNameOk = true;
			smtpSenderNameErr.innerHTML = '';
		}
	} else {
		smtpSenderNameOk = true;
		smtpSenderNameErr.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpSecureConnection = (element) => {
	const smtpSecureConnection = element
	smtpSecureConnectionOk = true;
	smtpSecureConnectionErr.innerHTML = '';

	checkSmtpForm();
}

const checkSmtpUseAuthentication = (element) => {
	const userElement = document.getElementById('smtp_user');
	const passElement = document.getElementById('smtp_pass');

	const smtpUseAuthentication = element;
	if (smtpUseAuthentication.value.trim() === 'true') {
		// userElement.removeAttribute('disabled');
		// passElement.removeAttribute('disabled');
		userElement.removeAttribute('disabled');
		passElement.removeAttribute('disabled');
	} else {
		// userElement.setAttribute('disabled', 'disabled');
		// passElement.setAttribute('disabled', 'disabled');
		userElement.setAttribute('disabled', 'disabled');
		passElement.setAttribute('disabled', 'disabled');
		smtpUseAuthenticationOk = true;
	}
	checkSmtpForm();
}

function setSelectedSmtpSecure(value) {
	const selectElement = document.getElementById("smtp_sso");

	if (selectElement) {
		selectElement.value = value.toString(); // Ensure it's a string
	}
}

const autodetectSmtp = async () => {
	const resultDiv = document.getElementById('result_div');
	resultDiv.innerHTML = "";
	const hostName = document.getElementById('smtp_host').value;
	const user = document.getElementById('smtp_user').value;
	const pass = document.getElementById('smtp_pass').value;
	document.getElementById('spinner_autodetect').classList.add('spinner');
	let autodetectResult = await detectSmtp(hostName, user, pass);
	document.getElementById('spinner_autodetect').classList.remove('spinner');
	console.log(autodetectResult);
	if (autodetectResult.successful) {
		resultDiv.classList.remove('error');
		resultDiv.classList.add('success');
		resultDiv.innerHTML = await localize(autodetectResult.message);

		document.getElementById('smtp_port').value = autodetectResult.port;
		document.getElementById('smtp_sso').value = autodetectResult.secure.toString();
		document.getElementById('auth_required').value = autodetectResult.requiresAuthentication;
		document.getElementById('smtp_port').style.borderColor = 'green';
		document.getElementById('smtp_sso').style.borderColor = 'green';
		document.getElementById('auth_required').style.borderColor = 'green';
		checkSmtpPort(document.getElementById('smtp_port'));
		checkSmtpUseAuthentication(document.getElementById('auth_required'));
		checkSmtpUser(document.getElementById('smtp_user'));
		checkSmtpPassword(document.getElementById('smtp_pass'));
		checkSmtpSenderEmail(document.getElementById('smtp_email'));
		checkSmtpSenderName(document.getElementById('smtp_name'));
		checkSmtpSecureConnection(document.getElementById('smtp_sso'));
		checkSmtpForm();
	} else {
		resultDiv.classList.remove('success');
		resultDiv.classList.add('error');
		resultDiv.innerHTML = await localize(autodetectResult.message);
	}
}

const testSmtpSettings = async () => {
	const hostVal = document.getElementById('smtp_host').value;
	const portVal = document.getElementById('smtp_port').value;
	const usernameVal = document.getElementById('smtp_user').value;
	const passwordVal = document.getElementById('smtp_pass').value;
	const senderEmailVal = document.getElementById('smtp_email').value;
	const senderNameVal = document.getElementById('smtp_name').value;
	const ssoVal = document.getElementById('smtp_sso').value;
	const authorizationRequiredVal = document.getElementById('auth_required').value;
	document.getElementById('spinner_test').classList.add('spinner');
	const testResult = await testSmtp(hostVal, portVal, usernameVal, passwordVal, senderEmailVal, senderNameVal, ssoVal, authorizationRequiredVal);
	console.log(testResult);
	document.getElementById('spinner_test').classList.remove('spinner');
	const resultDiv = document.getElementById('result_div');
	resultDiv.innerHTML = "";
	if (testResult.success) {
		resultDiv.classList.remove('error');
		resultDiv.classList.add('success');
		resultDiv.innerHTML = await localize("Testing email sent successfully");
		canSave = true;
		checkSmtpForm();
	} else {
		resultDiv.classList.remove('success');
		resultDiv.classList.add('error');
		resultDiv.innerHTML = await localize(testResult.error);
		canSave = false;
	}
	if (testResult.log) {
		console.log(testResult.log);
	}
}

window.onload = () => {
	checkSmtpHost(document.getElementById('smtp_host'));
	checkSmtpSenderEmail(document.getElementById('smtp_email'));
	checkSmtpSenderName(document.getElementById('smtp_name'));
}