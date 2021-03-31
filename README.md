# refactor-this

Add Logging i used a static Serilog implentation. I normaly use a proccessor per request and inject serilog but due to the time constrain i didn't have the time to set this up.
Did set up appsettings for the logging.

Seperated Products into 4 files.

I can't stand the the connections never closed so I used a using {} as i like that you can clearly see and define the scope.

I broke up the ProductController into productController and ProductOptionsController since they are different things i like a Single responciblity.

Add IHttpActionResults since it needs to return the correct status.

Removed the weird Constructor for product, products, productoptions and productoptions add a static.Load method since it has a better name and i think it looks a little cleaner.

Changed the connection string set up. normally i would put the connection string sin the configs and then look them up by the appkey and make a method to return the sql connection but i felt i didn't have the time. As i also like to include use a interface pattern and add loggin in each of the database calls.

Add Dapper so we no longer have to manualy build each object i think it cleans up the code and it looks nicer.

all so used hard code querys with paramater injection as i hate the insecurity that was there with the where being just concantanted on leaves you open to sql injection.

added to strings so the logging has the objects in it so you can see what is going on.
