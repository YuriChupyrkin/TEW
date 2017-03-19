export class ConstantStorage {
    private static userName: string;
    private static yandexTranslaterApiKey: string;
    private static userId: number;
    private static loadingEvent = 'loading';

    // urls
    private static readonly wordTranslaterController = '/api/WordTranslater';
    private static readonly wordsManagerController = '/api/WordsManager';
    private static readonly userInfoController = '/api/UserInfo';
    private static readonly applicationMessageController = '/api/ApplicationMessage';
    private static readonly userStatController = '/api/userStat';
    private static readonly deleteWordController = '/api/DeleteWord';
    private static readonly pickerTestsController = '/api/PickerTests';
    private static readonly wordsLevelUpdaterController = '/api/WordsLevelUpdater';
    private static readonly tewInfoContoller = '/api/TewInfo';
    private static readonly resetWordsLevelContoller = '/api/ResetWordsLevel';
    private static readonly editWordController = '/api/EditWord';

    public static getEditWordController() {
        return this.editWordController;
    }

    public static getResetWordsLevelController(){
        return this.resetWordsLevelContoller;
    }

    public static getTewInfoContoller(){
        return this.tewInfoContoller;
    }

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

    // end of urls

    public static getLoadingEvent() {
        return this.loadingEvent;
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