const title = document.getElementById("title");
const email = document.getElementById("email");
const password = document.getElementById("password");
const fname = document.getElementById("fname");
const lname = document.getElementById("lname");
let curUser = JSON.parse(sessionStorage.getItem("CurrentUser"))
title.textContent = `hello ${curUser.fname}`
email.textContent = `mail: ${curUser.mail}`
password.value = curUser.password
fname.value = curUser.fname
lname.value = curUser.lname
const Update = async () => {
    const updateData = {
        mail: curUser.mail,
        password: password.value,
        fname: fname.value,
        lname: lname.value
    }
    const responsePut = await fetch(`api/login/${curUser.userId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(updateData)
    })
    if (responsePut.ok) {
        alert("עדכון הפרטים בוצע בהצלחה")
    }
    else
        alert(responsePut.status)

}