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
	const smtpSenderEmail = document.getElementById('smtp_email');
	const smtpSenderName = document.getElementById('smtp_name');
	const smtpSecureConnection = document.getElementById('smtp_sso');
	const smtpUseAuthentication = document.getElementById('auth_required');
}