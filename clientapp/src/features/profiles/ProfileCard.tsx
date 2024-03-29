import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Card, Icon, Image } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import FollowButton from "./FollowButton";

interface Props {
	profile: Profile;
}

const maxCharactersInBio = 35;

export default observer(function ProfileCard({profile}: Props) {
	return (
		<Card 
			as={Link} to={`/profiles/${profile.username}`}>
			<Image src={profile.image || '/assets/user.png'} />
			<Card.Content>
				<Card.Header>{profile.displayName}</Card.Header>
				<Card.Description>{profile.bio && profile.bio?.length > maxCharactersInBio ? profile.bio?.substring(0, maxCharactersInBio) + '...' : profile.bio}</Card.Description>
			</Card.Content>
			<Card.Content extra>
				<Icon name="user" />
				{profile.followersCount} followers
			</Card.Content>
			<FollowButton profile={profile}></FollowButton>
		</Card>
	)	
});
