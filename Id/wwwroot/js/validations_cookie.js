const nameError = document.getElementById('cookie_name_err');
const domainError = document.getElementById('cookie_domain_err');
const pathError = document.getElementById('cookie_path_err');
const secureError = document.getElementById('cookie_secure_err');
const sameSiteError = document.getElementById('cookie_same_site_err');
const httpOnlyError = document.getElementById('cookie_http_only_err');
const expirationError = document.getElementById('cookie_expiration_err');
const submitButton = document.getElementById('submit');

let nameOk = false;
let domainOk = false;
let pathOk = false;
let secureOk = false;
let sameSiteOk = false;
let httpOnlyOk = false;
let expirationOk = false;

let formOk = false;

const checkForm = () => {
	(nameOk && domainOk && pathOk && secureOk && sameSiteOk && httpOnlyOk && expirationOk)
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

// check if the name is valid
const checkName = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, nameError, await localize('Name is required'));
		nameOk = false;
	} else {
		// check if the name is valid
		const regex = /^[a-zA-Z0-9_.]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, nameError, await localize('Name must contain only letters, numbers, underscores and dot'));
			nameOk = false;
		} else {
			setValid(element, nameError);
			nameOk = true;
		}
	}
	checkForm();
}

// check if the domain is valid
const checkDomain = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, domainError, await localize('Domain is required'));
		domainOk = false;
	} else {
		// check if the domain is valid
		const regex = /^[a-zA-Z0-9_.]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, domainError, await localize('Domain must contain only letters, numbers and underscores'));
			domainOk = false;
		} else {
			setValid(element, domainError);
			domainOk = true;
		}
	}
	checkForm();
}

const checkPath = async (element) => {
	if (element.value.length < 1) {
		element.value = '/';
	}

	// check if the path is valid
	const regex = /^[a-zA-Z0-9_./]+$/;
	if (!regex.test(element.value)) {
		setInvalid(element, pathError, await localize('Path must contain only letters, numbers and underscores'));
		pathOk = false;
	} else {
		setValid(element, pathError);
		pathOk = true;
	}
	checkForm();
}

const checkSecure = async (element) => {
	if (element.value == 'true') {
		setValid(element, secureError);
		secureOk = true;
	} else {
		setInvalid(element, secureError, await localize('This option is less secure'));
		secureOk = true;
	}
	checkForm();
}

const checkSameSite = async (element) => {
	if (element.value == '0') {
		setInvalid(element, sameSiteError, await localize('This option is less secure'));
		sameSiteOk = true;
	} else {
		setValid(element, sameSiteError);
		sameSiteOk = true;
	}
	checkForm();
}

const checkHttpOnly = async (element) => {
	if (element.value == 'true') {
		setValid(element, httpOnlyError);
		httpOnlyOk = true;
	} else {
		setInvalid(element, httpOnlyError, await localize('This option is less secure'));
		httpOnlyOk = true;
	}
	checkForm();
}

const checkExpiration = async (element) => {
	if (element.value < 0) {
		setInvalid(element, expirationError, await localize('The value must be a positive number or zero'));
		expirationOk = false;
	} else {
		setValid(element, expirationError);
		expirationOk = true;
	}
	checkForm();
}

const startup = async () => {
	const name = document.getElementById('cookie_name');
	const domain = document.getElementById('cookie_domain');
	const path = document.getElementById('cookie_path');
	const secure = document.getElementById('cookie_secure');
	const sameSite = document.getElementById('cookie_same_site');
	const httpOnly = document.getElementById('cookie_http_only');
	const expiration = document.getElementById('cookie_expiration');
	await checkName(name);
	await checkDomain(domain);
	await checkPath(path);
	await checkSecure(secure);
	await checkSameSite(sameSite);
	await checkHttpOnly(httpOnly);
	await checkExpiration(expiration);
}