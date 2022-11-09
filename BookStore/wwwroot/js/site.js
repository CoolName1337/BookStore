function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}


function showCheckboxes(id) {
    document.getElementById("checkboxes"+id).classList.toggle("hidden");
}

function showAdminPage(id) {
    var els = document.getElementsByClassName("admin-page");
    for (var i = 0; i < els.length; i++) {
        if (els[i].id.endsWith(id)) {
            els[i].classList.remove("hidden");
            continue;
        }
        els[i].classList.add("hidden");
    }
}
