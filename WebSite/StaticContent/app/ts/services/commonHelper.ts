export class CommonHelper {

    public static logOff() {
        if (confirm("log out?")) {
            window.location.href = '/account/SignOff';
        }
    }
}