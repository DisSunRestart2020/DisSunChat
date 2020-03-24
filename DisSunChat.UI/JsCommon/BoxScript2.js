$(function () {
    $("#txtGpu").html(getGlRenderer());
    $("#txtResolution").html(getResolution());
    $("#txtmodels").html(getModels());
    $("#txtUserAgent").html(GetUserAgent());
});
