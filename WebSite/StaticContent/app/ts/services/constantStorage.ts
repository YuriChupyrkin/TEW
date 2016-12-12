export class ConstantStorage {
    private static userName: string;
    private static yandexTranslaterApiKey: string;
    private static userId: number;

    // urls
    private static wordTranslaterController = '/api/WordTranslater';
    private static wordsManagerController = '/api/WordsManager';
    private static userInfoController = '/api/UserInfo';
    private static applicationMessageController = '/api/ApplicationMessage';
    private static userStatController = '/api/userStat';
    private static deleteWordController = '/api/DeleteWord';
    private static pickerTestsController = '/api/PickerTests';
    private static wordsLevelUpdaterController = 'api/WordsLevelUpdater';

    public static getWordsLevelUpdaterController(){
        return this.wordsLevelUpdaterController;
    }

    public static getPickerTestsController(){
        return this.pickerTestsController;
    }

    public static getDeleteWordController(){
        return this.deleteWordController;
    }

    public static getUserStatController(){
        return this.userStatController;
    }

    public static getApplicationMessageController(){
        return this.applicationMessageController;
    }

    public static getUserInfoController(){
        return this.userInfoController;
    }

    public static getWordTranslaterController(){
        return this.wordTranslaterController;
    }

    public static getWordsManagerController(){
        return this.wordsManagerController;
    }

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