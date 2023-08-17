import { WordAnswer } from "../dto/word-answer";

export interface WordResponse {
    wordId: number;
    word: string;
    meaning: string;
    percentage:string;
    writingInLanguage:string;
    isLearned:boolean;
    askWordAgain:boolean;
    wordAnswers: WordAnswer[];
}