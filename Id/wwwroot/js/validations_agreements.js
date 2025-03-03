// Track agreement status for terms, cookies, and data sharing
let termsAgreementOk = false;
let cookiesAgreementOk = false;
let datashareAgreementOk = false;

// Track the overall form validation status
let formOk = false;

// Get references to the error message elements and the submit button
const termsError = document.getElementById('terms_error');
const cookiesError = document.getElementById('cookies_error');
const datashareError = document.getElementById('datashare_error');
const submitButton = document.getElementById('submit');

/**
 * Checks the overall form validity by ensuring all agreements (terms, cookies, data sharing) are accepted.
 * Enables or disables the submit button based on the form status.
 */
const checkForm = () => {
	// If all agreements are accepted, form is valid
	(termsAgreementOk && cookiesAgreementOk && datashareAgreementOk)
		? formOk = true
		: formOk = false;

	// Enable or disable the submit button based on form validity
	formOk
		? submitButton.removeAttribute('disabled')
		: submitButton.setAttribute('disabled', 'disabled');
}

/**
 * Validates the Terms & Conditions agreement input.
 * Updates the element styling and error message, and triggers a full form validation check.
 * @param {HTMLElement} element - The input element for the Terms & Conditions agreement.
 */
const checkTerms = async (element) => {
	if (element.value == "true") {
		// User accepted terms
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		termsError.innerHTML = "";
		termsAgreementOk = true;
		checkForm();
	} else {
		// User did not accept terms
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		termsError.innerHTML = await localize(
			"Please accept the terms and conditions"
		);
		termsAgreementOk = false;
		checkForm();
	}
}

/**
 * Validates the Cookies agreement input.
 * Updates the element styling and error message, and triggers a full form validation check.
 * @param {HTMLElement} element - The input element for the Cookies agreement.
 */
const checkCookies = async (element) => {
	if (element.value == "true") {
		// User accepted cookies
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		cookiesError.innerHTML = "";
		cookiesAgreementOk = true;
		checkForm();
	} else {
		// User did not accept cookies
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		cookiesError.innerHTML = await localize(
			"Please accept cookies"
		);
		cookiesAgreementOk = false;
		checkForm();
	}
}

/**
 * Validates the Data Sharing agreement input.
 * Updates the element styling and error message, and triggers a full form validation check.
 * @param {HTMLElement} element - The input element for the Data Sharing agreement.
 */
const checkSharing = async (element) => {
	if (element.value == "true") {
		// User accepted data sharing
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		datashareError.innerHTML = "";
		datashareAgreementOk = true;
		checkForm();
	} else {
		// User did not accept data sharing
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		datashareError.innerHTML = await localize(
			"Please accept data sharing"
		);
		datashareAgreementOk = false;
		checkForm();
	}
}