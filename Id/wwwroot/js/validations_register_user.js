// important elements

// input elements
const email = document.getElementById("email_input");
const password = document.getElementById("password_input");
const passwordConfirm = document.getElementById("password_confirm_input");
const firstName = document.getElementById("first_name_input");
const lastName = document.getElementById("last_name_input");
const displayName = document.getElementById("displayed_name_input");
const birthdate = document.getElementById("birthdate_input");
const profilePicture = document.getElementById("profile_picture_input");
const acceptTerms = document.getElementById("accept_terms_input");
const acceptPrivacy = document.getElementById("accept_data_share");
const acceptCookies = document.getElementById("accept_cookies_input");
const submit = document.getElementById("submit_button");

// error elements
const emailError = document.getElementById("email_err");
const passwordError = document.getElementById("password_err");
const passwordConfirmError = document.getElementById("password_confirm_err");
const firstnameError = document.getElementById("firstname_err");
const lastnameError = document.getElementById("lastname_err");
const displayednameError = document.getElementById("displayedname_err");
const birthdateError = document.getElementById("birthdate_err");
const iconError = document.getElementById("icon_err");
const termsError = document.getElementById("terms_err");
const privacyError = document.getElementById("privacy_err");
const cookiesError = document.getElementById("cookies_err");

// check results
let emailOk = false;
let passwordOk = false;
let passwordConfirmOk = false;
let firstNameOk = false;
let lastNameOk = false;
let displayNameOk = false;
let birthdateOk = false;
let profilePictureOk = true;
let acceptTermsOk = false;
let acceptPrivacyOk = false;
let acceptCookiesOk = false;

// password rules
let passwordRules = null;

// element validation results
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
// form validation
const checkForm = () => {
	if (emailOk
		&& passwordOk
		&& passwordConfirmOk
		&& firstNameOk
		&& lastNameOk
		&& displayNameOk
		&& birthdateOk
		&& profilePictureOk
		&& acceptTermsOk
		&& acceptPrivacyOk
		&& acceptCookiesOk) {
		submit.removeAttribute('disabled');
	}
	else {
		submit.setAttribute('disabled', 'disabled');
	}
}

// validations
const validateEmail = async (element) => {
	const email = element.value;
	const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
	if (!emailPattern.test(email)) {
		setInvalid(element, emailError, await localize("Invalid email address"));
		emailOk = false;
		checkForm();
	}
	else {
		const result = await fetchData('/api/IsEmailUnique/' + email);
		if (result.success) {
			if (result.data.isUnique) {
				setValid(element, emailError);
				emailOk = true;
				checkForm();
			}
		}
	}
}

// password validation
const validatePassword = async (element) => {
	if (passwordRules == null) {
		let message = await localize("Password rules are not available");
		setInvalid(element, passwordError, message);
		let checks = document.getElementById("password_checks");
		checks.innerHTML = await localize("Password will be checked by the server");
		checks.style.borderColor = "orange";
		passwordOk = true;// password will be checked on the server
	}
	else {
		const password = element.value;
		const minChars = passwordRules.minimumLength;
		const maxChars = passwordRules.maximumLength;
		const requireLowercase = passwordRules.requireLowercase;
		const requireUppercase = passwordRules.requireUppercase;
		const requireDigit = passwordRules.requireDigit;
		const requireSpecial = passwordRules.requireNonAlphanumeric;

		passwordOk = true;
		// check minimal length
		if (password.length < minChars) {
			let message = await localize("Password must be at least") + " " + minChars + " " + await localize("characters long");
			setInvalid(element, passwordError, message);
			passChecker = document.getElementById('password_checks_minlength');
			passChecker.classList.remove("success");
			passChecker.classList.add("error");
			passwordOk = false;
		}
		else {
			setValid(element, passwordError);
			passChecker = document.getElementById('password_checks_minlength');
			passChecker.classList.remove("error");
			passChecker.classList.add("success");
		}

		// check maximal length
		if (password.length > maxChars) {
			let message = await localize("Password must be at most") + " " + maxChars + " " + await localize("characters long");
			setInvalid(element, passwordError, message);
			passChecker = document.getElementById('password_checks_maxlength');
			passChecker.classList.remove("success");
			passChecker.classList.add("error");
			passwordOk = false;
		}
		else {
			setValid(element, passwordError);
			passChecker = document.getElementById('password_checks_maxlength');
			passChecker.classList.remove("error");
			passChecker.classList.add("success");
		}

		// check lowercase
		if (requireLowercase && !/[a-z]/.test(password)) {
			let message = await localize("Password must contain at least one lowercase letter");
			setInvalid(element, passwordError, message);
			passChecker = document.getElementById('password_checks_lowercase');
			passChecker.classList.remove("success");
			passChecker.classList.add("error");
			passwordOk = false;
		}
		else if (!requireLowercase) {
		}
		else {
			setValid(element, passwordError);
			passChecker = document.getElementById('password_checks_lowercase');
			passChecker.classList.remove("error");
			passChecker.classList.add("success");
		}

		// check uppercase
		if (requireUppercase && !/[A-Z]/.test(password)) {
			let message = await localize("Password must contain at least one uppercase letter");
			setInvalid(element, passwordError, message);
			passChecker = document.getElementById('password_checks_uppercase');
			passChecker.classList.remove("success");
			passChecker.classList.add("error");
			passwordOk = false;
		}
		else if (!requireUppercase) {
		}
		else {
			setValid(element, passwordError);
			passChecker = document.getElementById('password_checks_uppercase');
			passChecker.classList.remove("error");
			passChecker.classList.add("success");
		}

		// check digit
		if (requireDigit && !/[0-9]/.test(password)) {
			let message = await localize("Password must contain at least one digit");
			setInvalid(element, passwordError, message);
			passChecker = document.getElementById('password_checks_digit');
			passChecker.classList.remove("success");
			passChecker.classList.add("error");
			passwordOk = false;
		}
		else if (!requireDigit) {
		}
		else {
			setValid(element, passwordError);
			passChecker = document.getElementById('password_checks_digit');
			passChecker.classList.remove("error");
			passChecker.classList.add("success");
		}

		// check special character
		if (requireSpecial && !/[^a-zA-Z0-9]/.test(password)) {
			let message = await localize("Password must contain at least one special character");
			setInvalid(element, passwordError, message);
			passChecker = document.getElementById('password_checks_special');
			passChecker.classList.remove("success");
			passChecker.classList.add("error");
			passwordOk = false;
		}
		else if (!requireSpecial) {
		}
		else {
			setValid(element, passwordError);
			passChecker = document.getElementById('password_checks_special');
			passChecker.classList.remove("error");
			passChecker.classList.add("success");
		}
	}
}

// password confirmation
const validatePasswordConfirm = async (element) => {
	const password = document.getElementById("password_input").value;
	const passwordConfirm = element.value;
	if (password !== passwordConfirm) {
		setInvalid(element, passwordConfirmError, await localize("Passwords do not match"));
		passwordConfirmOk = false;
	}
	else {
		setValid(element, passwordConfirmError);
		passwordConfirmOk = true;
	}
	checkForm();
}

// first name validation
const validateFirstName = async (element) => {
	const firstName = element.value;
	if (firstName.length < 2) {
		setInvalid(element, firstnameError, await localize("First name must be at least 2 characters long"));
		firstNameOk = false;
	}
	else {
		setValid(element, firstnameError);
		firstNameOk = true;
	}
	checkForm();
}

// last name validation
const validateLastName = async (element) => {
	const lastName = element.value;
	if (lastName.length < 2) {
		setInvalid(element, lastnameError, await localize("Last name must be at least 2 characters long"));
		lastNameOk = false;
	}
	else {
		setValid(element, lastnameError);
		lastNameOk = true;
	}
	checkForm();
}

// displayed name validation
const validateDisplayedName = async (element) => {
	const displayedName = element.value;
	if (displayedName.length < 2) {
		setInvalid(element, displayednameError, await localize("Displayed name must be at least 2 characters long"));
		displayNameOk = false;
	}
	else {
		setValid(element, displayednameError);
		displayNameOk = true;
	}
	checkForm();
}

// birthdate validation
const validateBirthdate = async (element) => {
	const birthdate = element.value;
	if (birthdate === "") {
		setInvalid(element, birthdateError, await localize("Birthdate is required"));
		birthdateOk = false;
	}
	else {
		setValid(element, birthdateError);
		birthdateOk = true;
	}
	checkForm();
}

// profile picture validation
const validateProfilePicture = async (element) => {
	const profilePicture = element.value;
	if (profilePicture === "") {
		setInvalid(element, iconError, await localize("Profile picture is required"));
		profilePictureOk = false;
	}
	else {
		setValid(element, iconError);
		profilePictureOk = true;
	}
	checkForm();
}

// terms validation
const validateTerms = async (element) => {
	if (!element.value == "true") {
		setInvalid(element, termsError, await localize("You must accept the terms"));
		acceptTermsOk = false;
	}
	else {
		setValid(element, termsError);
		acceptTermsOk = true;
	}
	checkForm();
}

// privacy validation
const validatePrivacy = async (element) => {
	if (!element.value == "true") {
		setInvalid(element, privacyError, await localize("You must accept the privacy policy"));
		acceptPrivacyOk = false;
	}
	else {
		setValid(element, privacyError);
		acceptPrivacyOk = true;
	}
	checkForm();
}

// cookies validation
const validateCookies = async (element) => {
	if (!element.value == "true") {
		setInvalid(element, cookiesError, await localize("You must accept the cookies policy"));
		acceptCookiesOk = false;
	}
	else {
		setValid(element, cookiesError);
		acceptCookiesOk = true;
	}
	checkForm();
}

const startup = async () => {
	let translation = await localize(description);
	passwordRules = await getPasswordRules();
	document.getElementById("description").innerHTML = translation;
	await validateEmail(email);
	await validatePassword(password);
	await validatePasswordConfirm(passwordConfirm);
	await validateFirstName(firstName);
	await validateLastName(lastName);
	await validateDisplayedName(displayName);
	await validateBirthdate(birthdate);
	await validateProfilePicture(profilePicture);
	await validateTerms(acceptTerms);
	await validatePrivacy(acceptPrivacy);
	await validateCookies(acceptCookies);
	checkForm();
}