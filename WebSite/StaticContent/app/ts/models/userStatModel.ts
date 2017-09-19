import { Word } from './word';

export class UserStatModel {
    LastActivityDate: string;
    ActivityLevel: number;
    WordsLevel: number;
    Email: string;
    NickName: string;
    Id: number;
    UniqueId: string;
    MostFailedWords: Array<Word>;
    MostStudiedWords: Array<Word>;
}