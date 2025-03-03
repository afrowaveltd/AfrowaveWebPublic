// References to input elements.
const inputs = {
	email: document.getElementById("email_input"),
	password: document.getElementById("password_input"),
	passwordConfirm: document.getElementById("password_confirm_input"),
	firstName: document.getElementById("first_name_input"),
	lastName: document.getElementById("last_name_input"),
	displayName: document.getElementById("displayed_name_input"),
	birthdate: document.getElementById("birthdate_input"),
	profilePicture: document.getElementById("profile_picture_input"),
	acceptTerms: document.getElementById("accept_terms_input"),
	acceptPrivacy: document.getElementById("accept_data_share"),
	acceptCookies: document.getElementById("accept_cookies_input"),
	submit: document.getElementById("submit_button"),
	iconPreview: document.getElementById("icon_preview"),
};

// References to error message containers.
const errors = {
	email: document.getElementById("email_err"),
	password: document.getElementById("password_err"),
	passwordConfirm: document.getElementById("password_confirm_err"),
	firstName: document.getElementById("firstname_err"),
	lastName: document.getElementById("lastname_err"),
	displayName: document.getElementById("displayedname_err"),
	birthdate: document.getElementById("birthdate_err"),
	icon: document.getElementById("icon_err"),
	terms: document.getElementById("terms_err"),
	privacy: document.getElementById("privacy_err"),
	cookies: document.getElementById("cookies_err"),
};

// Tracks validation status for each field.
const fieldStatus = {
	email: false,
	password: false,
	passwordConfirm: false,
	firstName: false,
	lastName: false,
	displayName: false,
	birthdate: false,
	profilePicture: true, // Optional - default to true
	acceptTerms: false,
	acceptPrivacy: false,
	acceptCookies: false,
};

let passwordRules = null;

/**
 * Sets field state to valid.
 */
const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}

/**
 * Sets field state to invalid with a provided message.
 */
const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

/**
 * Re-evaluates overall form validity and toggles submit button.
 */
const checkForm = () => {
	const allValid = Object.values(fieldStatus).every(status => status);
	inputs.submit.disabled = !allValid;
}

/**
 * Validates email using regex and checks uniqueness via API.
 */
const validateEmail = async () => {
	const email = inputs.email.value;
	const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

	if (!pattern.test(email)) {
		setInvalid(inputs.email, errors.email, await localize("Invalid email address"));
		fieldStatus.email = false;
	} else {
		const result = await fetchData(`/api/IsEmailUnique/${email}`);
		if (result.success && result.data.isUnique) {
			setValid(inputs.email, errors.email);
			fieldStatus.email = true;
		} else {
			setInvalid(inputs.email, errors.email, await localize("Email address is already registered"));
			fieldStatus.email = false;
		}
	}
	checkForm();
}

/**
 * Generic function to validate text inputs with min length.
 */
const validateText = async (field, minLen) => {
	if (inputs[field].value.length < minLen) {
		setInvalid(inputs[field], errors[field], await localize(`${field} must be at least ${minLen} characters long`));
		fieldStatus[field] = false;
	} else {
		setValid(inputs[field], errors[field]);
		fieldStatus[field] = true;
	}
	checkForm();
}

/**
 * Auto-populates display name if empty.
 */
const validateDisplayName = async () => {
	if (inputs.displayName.value.length < 2) {
		inputs.displayName.value = `${inputs.firstName.value} ${inputs.lastName.value}`;
		setInvalid(inputs.displayName, errors.displayName, await localize("If empty, your real name will be used"));
		fieldStatus.displayName = true; // Considered valid even with auto-populate
	} else {
		setValid(inputs.displayName, errors.displayName);
		fieldStatus.displayName = true;
	}
	checkForm();
}

/**
 * Validates birthdate.
 */
const validateBirthdate = async () => {
	const birthdate = new Date(inputs.birthdate.value);
	const today = new Date();

	if (!inputs.birthdate.value || birthdate > today || birthdate > new Date(today.getFullYear() - 8, today.getMonth(), today.getDate())) {
		setInvalid(inputs.birthdate, errors.birthdate, await localize("Invalid birthdate"));
		fieldStatus.birthdate = false;
	} else {
		setValid(inputs.birthdate, errors.birthdate);
		fieldStatus.birthdate = true;
	}
	checkForm();
}

/**
 * Validates checkbox fields like terms, privacy, cookies.
 */
const validateCheckbox = async (field) => {
	if (inputs[field].value !== "true") {
		setInvalid(inputs[field], errors[field], await localize(`You must accept the ${field}`));
		fieldStatus[field] = false;
	} else {
		setValid(inputs[field], errors[field]);
		fieldStatus[field] = true;
	}
	checkForm();
}

/**
 * Example listener setup to validate on input.
 */
const setupListeners = () => {
	inputs.email.addEventListener('input', validateEmail);
	inputs.firstName.addEventListener('change', () => validateText('firstName', 2));
	inputs.lastName.addEventListener('change', () => validateText('lastName', 2));
	inputs.displayName.addEventListener('change', validateDisplayName);
	inputs.birthdate.addEventListener('input', validateBirthdate);
	['acceptTerms', 'acceptPrivacy', 'acceptCookies'].forEach(field =>
		inputs[field].addEventListener('input', () => validateCheckbox(field))
	);
}

/**
 * Initial startup process.
 */
const startup = async () => {
	document.getElementById("description").innerHTML = await localize(description);
	passwordRules = await getPasswordRules();

	await validateEmail();
	await validateText('firstName', 2);
	await validateText('lastName', 2);
	await validateDisplayName();
	await validateBirthdate();

	await validateCheckbox('acceptTerms');
	await validateCheckbox('acceptPrivacy');
	await validateCheckbox('acceptCookies');

	checkForm();
	setupListeners();
}