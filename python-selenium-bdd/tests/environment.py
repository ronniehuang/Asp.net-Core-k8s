from framework.webapp import webapp

def after_scenario(context, step):
    #print('after_scenario')
    if step.status == 'failed':
        webapp.make_screenshot()

# def before_feature(context, feature):
    # #print("before_feature activated")

# def after_feature(context, feature):
    # #print("after_feature activated")

# def before_tag(context, tag):
    # #print("before_tag activated")

# def before_scenario(context, scenario):
    # #print("before_scenario activated")

# def before_step(context, step):
    # #print("before_step activated")

# def after_step(context, step):
    # #print("after_step activated")

# def after_tag(context, tag):
    # #print("after_tag activated")
    
# def before_all(context):
	# #print("before_all activated")
    
def after_all(context):
	webapp.driver.quit()
