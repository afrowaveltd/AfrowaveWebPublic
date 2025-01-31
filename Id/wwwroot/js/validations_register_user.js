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
const acceptPrivacy = document.getElementById("accept_privacy_input");
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

const startup = async () => {
	let translation = await localize(description);
	passwordRules = await getPasswordRules();
	document.getElementById("description").innerHTML = translation;
}