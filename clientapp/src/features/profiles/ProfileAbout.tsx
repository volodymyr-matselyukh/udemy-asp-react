import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { Button, Grid, Header, Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import ProfileAboutForm from "./ProfileAboutForm";

interface Props {
	profile: Profile;
}

export default observer(function ProfileAbout({ profile }: Props) {
	const [isEditMode, setIsEditMode] = useState(false);

	const closeEditMode = () => {
		setIsEditMode(false);
	}

	return (
		<Tab.Pane>
			<Grid>
				<Grid.Column width={8}>
					<Header
						floated="left"
						icon="user"
						content={`About ${profile.displayName}`}
					/>
				</Grid.Column>
				<Grid.Column width={8}>
					<Button basic content={isEditMode ? 'Cancel' : 'Edit Profile'} floated="right"
						onClick={() => setIsEditMode(!isEditMode)}
					/>
				</Grid.Column>
				<Grid.Column width={16}>
					{
						isEditMode ?
						(
							<ProfileAboutForm profile={profile} closeEditMode={closeEditMode} />
						) : (
							<p style={{whiteSpace:"pre-wrap"}}>{profile.bio}</p>
						)
					}
				</Grid.Column>
			</Grid>
		</Tab.Pane>
	);
});
