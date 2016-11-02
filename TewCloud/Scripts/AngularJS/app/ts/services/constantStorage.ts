export class ConstantStorage {
    private static userName: string;
    private static yandexTranslaterApiKey: string;

    public static setUserName(name: string) {
        this.userName = name;
    }

    public static getUserName() {
        return this.userName;
    }

    public static setYandexTranslaterApiKey(apiKey: string) {
        this.yandexTranslaterApiKey = apiKey;
    }

    public static getYandexTranslaterApiKey() {
        return this.yandexTranslaterApiKey;
    }
}