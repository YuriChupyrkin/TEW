export class PubSub {
   private static registry = {};

   public static Pub (name: string, ...args: Array<any>) {
        if (!this.registry[name]) {
            return;
        }

        this.registry[name].forEach(x => {
            x.apply(null, args);
        });
    }

    public static Sub (name: string, fn: any) {
        if (this.registry && name && fn) {

            if (!this.registry[name]) {
                this.registry[name] = [fn];
            } else if (this.registry[name].length) {
                this.registry[name].push(fn);
            }
        }
    }

    public static Clear(name: string) {
        if (this.registry && name) {
            this.registry[name] = null;
        }
    }
}