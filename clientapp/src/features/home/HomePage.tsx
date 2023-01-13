import FacebookLogin from "@greatsumini/react-facebook-login";
import { observer } from "mobx-react-lite";
import { Link, ScrollRestoration } from "react-router-dom";
import { Button, Container, Divider, Header, Image, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer(function HomePage() {
	const { userStore, modalStore } = useStore();

	return (
		<Segment inverted textAlign="center" vertical className="masthead">
			<Container text>
				<Header as="h1" inverted>
					<Image
						size="massive"
						src="/assets/logo.png"
						alt="logo"
						style={{ marginBotton: 12 }}
					/>
					Reactivities
				</Header>
				{userStore.isLoggedIn ? (
					<>
						<Header
							as="h2"
							inverted
							content="Welcome to Reactivities"
						/>
						<Button as={Link} to="/activities" size="huge" inverted>
							Go to Activities!
						</Button>
					</>
				) : (
					<>
						<Button
							onClick={() => modalStore.openModal(<LoginForm />)}
							size="huge"
							inverted
						>
							Login!
						</Button>
						<Button
							onClick={() =>
								modalStore.openModal(<RegisterForm />)
							}
							size="huge"
							inverted
						>
							Register
						</Button>

						<Divider horizontal inverted></Divider>
						<Button 
							as={FacebookLogin}
							appId='852245359214694'
							size='huge'
							inverted
							color='facebook'
							content='Login with Facebook'
							onSuccess={(response: any) => {
								console.log('login success', response);
							}}
							onFail={(response: any) => {
								console.log('login failed', response);
							}}
						/>
					</>
				)}
			</Container>
		</Segment>
	);
});
