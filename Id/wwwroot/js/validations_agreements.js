let termsAgreementOk = false;
let cookiesAgreementOk = false;
let datashareAgreementOk = false;

let formOk = false;

const termsError = document.getElementById('terms_error');
const cookiesError = document.getElementById('cookies_error');
const datashareError = document.getElementById('datashare_error');
const submitButton = document.getElementById('submit');

const checkForm = () => {
    (termsAgreementOk && cookiesAgreementOk && datashareAgreementOk)
        ? formOk = true
        : formOk = false;

    formOk
        ? submit.removeAttribute('disabled')
        : submit.setAttribute('disabled', 'disabled');
}

const checkTerms = async (element) => {
    if (element.value == "true") {
        element.classList.remove("input-invalid");
        element.classList.add("input-valid");
        termsError.innerHTML = "";
        termsAgreementOk = true;
        checkForm();
    } else {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        termsError.innerHTML = await localize(
            "Please accept the terms and conditions"
        );
        termsAgreementOk = false;
        checkForm();
    }
}

const checkCookies = async (element) => {
    if (element.value == "true") {
        element.classList.remove("input-invalid");
        element.classList.add("input-valid");
        cookiesError.innerHTML = "";
        cookiesAgreementOk = true;
        checkForm();
    } else {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        cookiesError.innerHTML = await localize(
            "Please accept cookies"
        );
        cookiesAgreementOk = false;
        checkForm();
    }
}

const checkSharing = async (element) => {
    if (element.value == "true") {
        element.classList.remove("input-invalid");
        element.classList.add("input-valid");
        datashareError.innerHTML = "";
        datashareAgreementOk = true;
        checkForm();
    } else {
        element.classList.remove("input-valid");
        element.classList.add("input-invalid");
        datashareError.innerHTML = await localize(
            "Please accept data sharing"
        );
        datashareAgreementOk = false;
        checkForm();
    }
}