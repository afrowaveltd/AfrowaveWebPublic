// Track validation states for all SMTP fields
const smtpStatus = {
	host: false,
	port: true, // default
	user: false,
	password: false,
	senderEmail: false,
	senderName: true, // default
	secureConnection: true, // default
	useAuthentication: true, // default
};

// Track form capabilities
let canAutodetect = false;
let canTest = false;
let canSave = false;

// References to error message containers
const errors = {
	host: document.getElementById('smtp_host_err'),
	port: document.getElementById('smtp_port_err'),
	user: document.getElementById('smtp_user_err'),
	password: document.getElementById('smtp_pass_err'),
	senderEmail: document.getElementById('smtp_email_err'),
	senderName: document.getElementById('smtp_name_err'),
	secureConnection: document.getElementById('smtp_sso_err'),
};

// References to UI elements
const logDiv = document.getElementById('test_log');
const logData = document.getElementById('log_content');
const logTitle = document.getElementById('log_title');

// Unified check form function to handle all state and button toggling
const checkSmtpForm = () => {
	const allValid = Object.values(smtpStatus).every(status => status);
	const authRequired = document.getElementById('auth_required').value === "true";

	canTest = allValid;
	canAutodetect = smtpStatus.host && authRequired;

	toggleButton('testSmtp', canTest);
	toggleButton('detectSmtp', canAutodetect);
	toggleButton('submit', canSave);

	// Visual feedback (border colors)
	applyFieldStatus('smtp_host', smtpStatus.host);
	applyFieldStatus('smtp_port', smtpStatus.port);
	applyFieldStatus('smtp_user', smtpStatus.user);
	applyFieldStatus('smtp_pass', smtpStatus.password);
	applyFieldStatus('smtp_email', smtpStatus.senderEmail);
	applyFieldStatus('smtp_name', smtpStatus.senderName);
	applyFieldStatus('smtp_sso', smtpStatus.secureConnection);
	applyFieldStatus('auth_required', smtpStatus.useAuthentication);
}

// Helper: Set input field border color based on its validation status
const applyFieldStatus = (fieldId, isValid) => {
	document.getElementById(fieldId).style.borderColor = isValid ? 'green' : 'red';
}

// Helper: Enable or disable a button
const toggleButton = (buttonId, enabled) => {
	document.getElementById(buttonId).disabled = !enabled;
}

// Generic field checker for simple required fields
const checkRequiredField = async (field, errorElement, errorMessageKey) => {
	if (field.value.trim() === '') {
		smtpStatus[field.id.split('_')[1]] = false;
		errorElement.innerHTML = await localize(errorMessageKey);
	} else {
		smtpStatus[field.id.split('_')[1]] = true;
		errorElement.innerHTML = '';
	}
	checkSmtpForm();
}

// Individual field checkers
const checkSmtpHost = async (element) => checkRequiredField(element, errors.host, 'SMTP Host is required');
const checkSmtpPort = async (element) => checkRequiredField(element, errors.port, 'SMTP Port is required');

const checkSmtpUser = async (element) => {
	const authRequired = document.getElementById('auth_required').value === "true";
	if (element.value.trim() === '' && authRequired) {
		smtpStatus.user = false;
		errors.user.innerHTML = await localize('SMTP user is required');
	} else {
		smtpStatus.user = true;
		errors.user.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpPassword = async (element) => {
	const authRequired = document.getElementById('auth_required').value === "true";
	if (element.value.trim() === '' && authRequired) {
		smtpStatus.password = false;
		errors.password.innerHTML = await localize('SMTP Password is required');
	} else {
		smtpStatus.password = true;
		errors.password.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpSenderEmail = async (element) => checkRequiredField(element, errors.senderEmail, 'Sender Email is required');

const checkSmtpSenderName = async (element) => {
	if (element.value.trim() === '') {
		const email = document.getElementById('smtp_email').value;
		if (email.trim() === '') {
			smtpStatus.senderName = false;
			errors.senderName.innerHTML = await localize('Sender name is required');
		} else {
			element.value = email;
			smtpStatus.senderName = true;
			errors.senderName.innerHTML = '';
		}
	} else {
		smtpStatus.senderName = true;
		errors.senderName.innerHTML = '';
	}
	checkSmtpForm();
}

const checkSmtpSecureConnection = () => {
	smtpStatus.secureConnection = true;
	errors.secureConnection.innerHTML = '';
	checkSmtpForm();
}

const checkSmtpUseAuthentication = () => {
	const userField = document.getElementById('smtp_user');
	const passField = document.getElementById('smtp_pass');
	const authRequired = document.getElementById('auth_required').value === 'true';

	userField.disabled = !authRequired;
	passField.disabled = !authRequired;

	smtpStatus.useAuthentication = true; // This doesn't need explicit validation
	checkSmtpForm();
}

// Function to set selected secure connection option (used by autodetect)
function setSelectedSmtpSecure(value) {
	document.getElementById("smtp_sso").value = value.toString();
}

// Autodetect SMTP settings from the server
const autodetectSmtp = async () => {
	const resultDiv = document.getElementById('result_div');
	resultDiv.innerHTML = '';

	const host = document.getElementById('smtp_host').value;
	const user = document.getElementById('smtp_user').value;
	const pass = document.getElementById('smtp_pass').value;

	document.getElementById('spinner_autodetect').classList.add('spinner');
	const autodetectResult = await detectSmtp(host, user, pass);
	document.getElementById('spinner_autodetect').classList.remove('spinner');

	resultDiv.classList.remove('error', 'success');
	resultDiv.classList.add(autodetectResult.successful ? 'success' : 'error');
	resultDiv.innerHTML = await localize(autodetectResult.message);

	if (autodetectResult.successful) {
		document.getElementById('smtp_port').value = autodetectResult.port;
		setSelectedSmtpSecure(autodetectResult.secure);
		document.getElementById('auth_required').value = autodetectResult.requiresAuthentication;

		checkSmtpForm();
	}
}

// Test SMTP settings
const testSmtpSettings = async () => {
	const testTarget = prompt(await localize("Enter the email address to send the test email to"), document.getElementById('smtp_email').value);
	if (!testTarget) return;

	document.getElementById('spinner_test').classList.add('spinner');
	const testResult = await testSmtp(
		...['host', 'port', 'user', 'password', 'senderEmail', 'senderName', 'secureConnection', 'useAuthentication']
			.map(field => document.getElementById(`smtp_${field}`).value),
		testTarget
	);
	document.getElementById('spinner_test').classList.remove('spinner');

	logDiv.style.display = 'block';
	logTitle.innerHTML = await localize("SMTP testing result");
	showLogInElement(testResult.log, 'log_content');

	canSave = testResult.success;
	checkSmtpForm();
}

// On load initialization
window.onload = () => {
	checkSmtpHost(document.getElementById('smtp_host'));
	checkSmtpSenderEmail(document.getElementById('smtp_email'));
	checkSmtpSenderName(document.getElementById('smtp_name'));
	logDiv.style.display = 'none';
}