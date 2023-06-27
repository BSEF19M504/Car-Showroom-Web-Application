function validateLogin() {
    var email = document.getElementById("inputEmail").value;
    var pass = document.getElementById("inputPassword").value;
    var err = 0;
    if (pass === "") {
        document.getElementById("pas").innerHTML = "Password cannot be empty!";
        err++;
    }
    else {
        document.getElementById("pas").innerHTML = "";
    }
    if (email === "") {
        document.getElementById("ema").innerHTML = "Email cannot be empty!";
        err++;
    }
    else {
        document.getElementById("ema").innerHTML = "";
    }
    if (err === 0) {
        return true;
    }
    return false;
}

function validateCar(event) {
    if (event.submitter.name === "cancel") {
        return true;
    }
    var make = document.getElementById("inputMake").value;
    var model = document.getElementById("inputModel").value;
    var year = document.getElementById("inputYear").value;
    var mileage = document.getElementById("inputMileage").value;
    var price = document.getElementById("inputPrice").value;
    var err = 0;
    if (make === "") {
        document.getElementById("mak").innerHTML = "Make cannot be empty!";
        err++;
    }
    else {
        document.getElementById("mak").innerHTML = "";
    }
    if (model === "") {
        document.getElementById("mod").innerHTML = "Model cannot be empty!";
        err++;
    }
    else {
        document.getElementById("mod").innerHTML = "";
    }
    if (year === "") {
        document.getElementById("yer").innerHTML = "Year cannot be empty!";
        err++;
    }
    else {
        document.getElementById("yer").innerHTML = "";
    }
    if (mileage === "") {
        document.getElementById("mil").innerHTML = "Mileage cannot be empty!";
        err++;
    }
    else {
        document.getElementById("mil").innerHTML = "";
    }
    if (price === "") {
        document.getElementById("pri").innerHTML = "Price cannot be empty!";
        err++;
    }
    else {
        document.getElementById("pri").innerHTML = "";
    }
    if (err === 0) {
        return true;
    }
    return false;
}

function letterNumber(event) {
    var keychar;

    keychar = event.key;

    // alphas and numbers
    if (((".+-0123456789").indexOf(keychar) > -1))
        return true;
    else
        return false;
}

function search() {
    let name = document.getElementById("textCars").value;
    window.location.href = "/Cars/GetCar?name=" + name;
}