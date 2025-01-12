const issuerError = document.getElementById('issuer_err');
const audienceError = document.getElementById('audience_err');
const accessTokenExpirationError = document.getElementById('access_token_expiration_err');
const refreshTokenExpirationError = document.getElementById('refresh_token_expiration_err');
const submitButton = document.getElementById('submit');

let issuerOk = false;
let audienceOk = false;
let accessTokenExpirationOk = false;
let refreshTokenExpirationOk = false;

let formOk = false;

const checkForm = () => {
	console.log("checking form");
	(issuerOk && audienceOk && accessTokenExpirationOk && refreshTokenExpirationOk)
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

// check if the issuer is valid
const checkIssuer = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, issuerError, await localize('Issuer is required'));
		issuerOk = false;
	} else {
		// check if the issuer is valid
		const regex = /^[a-zA-Z0-9_.]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, issuerError, await localize('Issuer must contain only letters, numbers, underscores and dot'));
			issuerOk = false;
		} else {
			setValid(element, issuerError);
			issuerOk = true;
		}
	}
	checkForm();
}

// check if the audience is valid
const checkAudience = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, audienceError, await localize('Audience is required'));
		audienceOk = false;
	} else {
		// check if the audience is valid
		const regex = /^[a-zA-Z0-9_.*/]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, audienceError, await localize('Audience must contain only letters, numbers, underscores, dashes, slashes and dot'));
			audienceOk = false;
		} else {
			setValid(element, audienceError);
			audienceOk = true;
		}
	}
	checkForm();
}

// check if the access token expiration is valid
const checkAccessTokenExpiration = async (element) => {
	if (element.value < 0) {
		setInvalid(element, accessTokenExpirationError, await localize('The value must be a positive number or zero'));
		accessTokenExpirationOk = false;
	} else {
		setValid(element, accessTokenExpirationError);
		accessTokenExpirationOk = true;
	}
	checkForm();
}

// check if the refresh token expiration is valid
const checkRefreshTokenExpiration = async (element) => {
	if (element.value < 0) {
		setInvalid(element, refreshTokenExpirationError, await localize('The value must be a positive number or zero'));
		refreshTokenExpirationOk = false;
	} else {
		setValid(element, refreshTokenExpirationError);
		refreshTokenExpirationOk = true;
	}
	checkForm();
}

const startup = async () => {
	console.log("Starting...");
	const issuer = document.getElementById('issuer');
	const audience = document.getElementById('audience');
	const accessTokenExpiration = document.getElementById('access_token');
	const refreshTokenExpiration = document.getElementById('refresh_token');
	await checkIssuer(issuer);
	await checkAudience(audience);
	await checkAccessTokenExpiration(accessTokenExpiration);
	await checkRefreshTokenExpiration(refreshTokenExpiration);
}