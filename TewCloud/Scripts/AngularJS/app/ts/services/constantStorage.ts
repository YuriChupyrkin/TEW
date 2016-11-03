export class ConstantStorage {
    private static userName: string;
    private static yandexTranslaterApiKey: string;
    private static userId: number;

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

    public static setUserId(id: number) {
        this.userId = id;
    }

    public static getUserId() {
        return this.userId;
    }
}