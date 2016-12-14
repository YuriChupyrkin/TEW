declare var jQuery: any;

export class JQueryHelper {
    public static getElement(expression: string){
        return jQuery(expression);
    }
    
    public static getElementById(id: string){
        return this.getElement(`#${id}`);
    }
}