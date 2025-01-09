const maxFailedError = document.getElementById('login_rules_err_max_failed');
const lockoutTimeError = document.getElementById('login_rules_err_lockout_time');
const passwordTokenError = document.getElementById('login_rules_err_password_reset_token_expiration');
const emailTokenError = document.getElementById('login_rules_err_email_token_expiration');
const requireConfirmation = document.getElementById('login_rules_err_require_email_confirmation');
const submitButton = document.getElementById('submit');

let maxFailedOk = true;
let lockoutTimeOk = true;
let passwordTokenOk = true;
let emailTokenOk = true;
let requireConfirmationOk = true;

let warning = '';

const checkForm = () => {
	(maxFailedOk && lockoutTimeOk && passwordTokenOk && emailTokenOk && requireConfirmationOk)
		? formOk = true
		: formOk = false;
	formOk
		? submitButton.removeAttribute('disabled')
		: submitButton.setAttribute('disabled', 'disabled');
}

const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}
const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

const checkMaxFailed = async (element) => {
	if (element.value < 0) {
		warning = await localize("The value must be a positive number or zero");
		setInvalid(element, maxFailedError, warning);
		maxFailedOk = false;
	}
	else if (element.value == 0) {
		warning = await localize("The max failed login attempts set to 0 will disable account locking out");
		setInvalid(element, maxFailedError, warning);
		maxFailedOk = true;
	}
	else if (element.value < 3) {
		warning = await localize("The maximum failed login attempts set to less than 3 is not recommended");
		setInvalid(element, maxFailedError, warning);
		maxFailedOk = true;
	}
	else {
		warning = '';
		setValid(element, maxFailedError);
		maxFailedOk = true;
	}
	checkForm();
}

const checkLockoutTime = async (element) => {
	if (element.value < 0) {
		warning = await localize("The value must be a positive number or zero");
		setInvalid(element, lockoutTimeError, warning);
		lockoutTimeOk = false;
	}
	else if (element.value == 0) {
		warning = await localize("The account lockout time set to 0 will lock out accounts indefinitely");
		setInvalid(element, lockoutTimeError, warning);
		lockoutTimeOk = true;
	}
	else {
		warning = '';
		setValid(element, lockoutTimeError);
		lockoutTimeOk = true;
	}
	checkForm();
}

const checkPasswordTokenExpiration = async (element) => {
	if (element.value < 0) {
		warning = await localize("The value must be a positive number or zero");
		setInvalid(element, passwordTokenError, warning);
		passwordTokenOk = false;
	}
	else if (element.value == 0) {
		warning = await localize("The password reset token expiration set to 0 will make token valid until it is used");
		setInvalid(element, passwordTokenError, warning);
		passwordTokenOk = true;
	}
	else {
		warning = '';
		setValid(element, passwordTokenError);
		passwordTokenOk = true;
	}
	checkForm();
}

const checkEmailTokenExpiration = async (element) => {
	if (element.value < 0) {
		warning = await localize("The value must be a positive number or zero");
		setInvalid(element, emailTokenError, warning);
		emailTokenOk = false;
	}
	else if (element.value == 0) {
		warning = await localize("The email confirmation token expiration set to 0 will make token valid until it is used");
		setInvalid(element, emailTokenError, warning);
		emailTokenOk = true;
	}
	else {
		warning = '';
		setValid(element, emailTokenError);
		emailTokenOk = true;
	}
	checkForm();
}

const checkRequireConfirmedEmail = async (element) => {
	if (element.value == "true") {
		warning = '';
		setValid(element, requireConfirmation);
		requireConfirmationOk = true;
	}
	else {
		warning = await localize("Disabling email confirmation will allow users to login without confirming their email");
		setInvalid(element, requireConfirmation, warning);
		requireConfirmationOk = true;
	}
	checkForm();
}